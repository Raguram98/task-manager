using Azure;
using Azure.Communication.Email;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace DueTaskNotificationFunction;

public class CheckDueTasks
{
    private readonly ILogger _logger;

    public CheckDueTasks(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<CheckDueTasks>();
    }

    [Function("CheckDueTasks")]
    public async Task Run([TimerTrigger("0 0 8 * * *")] TimerInfo myTimer) // Checks daily at 8 AM
    {
        _logger.LogInformation("CheckDueTasks function started at: {executionTime}", DateTime.Now);

        try
        {
            // Get secrets from Key Vault
            var connectionString = await GetSecretFromKeyVault("AzureCommunicationServicesConnectionString");
            var senderEmail = await GetSecretFromKeyVault("AzureCommunicationServicesSenderEmail");
            var dbConnectionString = Environment.GetEnvironmentVariable("SQLDB_CONNECTION_STRING");

            if (string.IsNullOrEmpty(dbConnectionString))
            {
                _logger.LogError("SQLDB_CONNECTION_STRING is not set");
                return;
            }

            // Get tasks due today
            var dueTasks = await GetDueTasksFromDatabase(dbConnectionString);
            _logger.LogInformation("Found {count} tasks due today", dueTasks.Count);

            // Send emails for each due task
            var emailClient = new EmailClient(connectionString);
            int emailsSent = 0;

            foreach (var task in dueTasks)
            {
                try
                {
                    await SendDueNotificationEmail(emailClient, senderEmail, task);
                    emailsSent++;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Failed to send email for task {taskId}: {error}", task.Id, ex.Message);
                }
            }

            _logger.LogInformation("Successfully sent {count} due date notifications", emailsSent);

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation("Next timer schedule: {nextSchedule}", myTimer.ScheduleStatus.Next);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("CheckDueTasks function failed: {error}", ex.Message);
            throw;
        }
    }

    private async Task<string> GetSecretFromKeyVault(string secretName)
    {
        var keyVaultUrl = Environment.GetEnvironmentVariable("KEY_VAULT_URL")
            ?? throw new InvalidOperationException("KEY_VAULT_URL not set");

        var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
        KeyVaultSecret secret = await client.GetSecretAsync(secretName);
        return secret.Value;
    }

    private async Task<List<(string Id, string UserEmail, string Title)>> GetDueTasksFromDatabase(string connectionString)
    {
        var tasks = new List<(string, string, string)>();
        var today = DateOnly.FromDateTime(DateTime.Now);

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();

            string query = @"
                SELECT t.Id, u.Email, t.Title
                FROM Tasks t
                INNER JOIN Users u ON t.UserId = u.Id
                WHERE CAST(t.DueDate AS DATE) = @today
                AND t.IsCompleted = 0";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@today", today.ToDateTime(TimeOnly.MinValue));

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var id = reader["Id"]?.ToString() ?? "";
                        var email = reader["Email"]?.ToString() ?? "";
                        var title = reader["Title"]?.ToString() ?? "";

                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(email))
                        {
                            tasks.Add((id, email, title));
                        }
                    }
                }
            }
        }

        return tasks;
    }

    private async Task SendDueNotificationEmail(EmailClient emailClient, string senderEmail, (string Id, string UserEmail, string Title) task)
    {
        try
        {
            var emailContent = new EmailContent(
                subject: $"Reminder: '{task.Title}' is due today"
            )
            {
                PlainText = $"Hi,\n\nYour task '{task.Title}' is due today. Please complete it at your earliest convenience.\n\nBest regards,\nTask Manager"
            };

            var emailMessage = new EmailMessage(
                senderAddress: senderEmail,
                content: emailContent,
                recipients: new EmailRecipients(new List<EmailAddress> { new(task.UserEmail) })
            );

            _logger.LogInformation("Sending email to {email} for task {taskId}", task.UserEmail, task.Id);
            await emailClient.SendAsync(WaitUntil.Completed, emailMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error sending email: {error}", ex.Message);
            throw;
        }
    }
}
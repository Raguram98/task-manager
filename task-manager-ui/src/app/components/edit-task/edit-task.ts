import { Component, inject, OnInit, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { TaskService } from '../../services/task.service';
import { ToastService } from '../../services/toast.service';

@Component({
  selector: 'app-edit-task',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './edit-task.html',
  styleUrl: './edit-task.css',
})
export class EditTask implements OnInit {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private taskService = inject(TaskService);
  private router = inject(Router);
  private toast = inject(ToastService);

  loading = signal(true);
  taskId  = '';

  // form starts undefined — we build it after the task loads
  form = this.fb.group({
    title:       ['', [Validators.required, Validators.minLength(2)]],
    description: [''],  
    isCompleted: [false],
    dueDate: ['']
  });

  get titleCtrl() { return this.form.get('title')!; }

  ngOnInit() {
    // ActivatedRoute reads the :id from the URL  (e.g. /edit/3 → id = 3)
    this.taskId = this.route.snapshot.paramMap.get('id')!;

    this.taskService.getTask(this.taskId).subscribe({
      next: task => {
        // Patch fills the form fields with existing task data
        this.form.patchValue({
          title:       task.title,
          description: task.description,
          isCompleted: task.isCompleted,
          dueDate:     task.dueDate  
        });
        this.loading.set(false);
      },
      error: () => {
        this.toast.error('Could not load task.');
        this.router.navigate(['/']);
      }
    });
  }

  updateTask() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    const payload = { id: this.taskId, ...this.form.value } as any;
    this.taskService.updateTask(this.taskId, payload).subscribe({
      next: () => { this.toast.success('Task updated!'); this.router.navigate(['/tasks']); },
      error: ()  => this.toast.error('Failed to update task.')
    });
  }

  goBack() { this.router.navigate(['/']); }
}
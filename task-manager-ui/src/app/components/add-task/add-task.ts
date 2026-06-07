import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { TaskService } from '../../services/task.service';
import { ToastService } from '../../services/toast.service';

@Component({
  selector: 'app-add-task',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './add-task.html',
  styleUrl: './add-task.css',
})
export class AddTask {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private taskService = inject(TaskService);
  private toast = inject(ToastService);

  // FormBuilder creates a reactive form.
  // Each field gets: [initialValue, [validators]]
  // Validators run automatically — no manual if/else checks needed.
  form = this.fb.group({
    title:       ['', [Validators.required, Validators.minLength(2)]],
    description: ['']
  });

  // Shortcut so the template can access form.get('title') more cleanly
  get titleCtrl() { return this.form.get('title')!; }

  addTask() {
    if (this.form.invalid) {
      this.form.markAllAsTouched(); // shows validation errors
      return;
    }
    const payload = { ...this.form.value, id: 0, isCompleted: false } as any;
    this.taskService.addTask(payload).subscribe({
      next: () => { this.toast.success('Task created!'); this.router.navigate(['/tasks']); },
      error: ()  => this.toast.error('Failed to create task.')
    });
  }

  goBack() { this.router.navigate(['/tasks']); }
}
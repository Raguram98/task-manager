import { Component, inject, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { TaskService } from '../../services/task.service';
import { ToastService } from '../../services/toast.service';
import { TaskItem } from '../../models/task.model';

type Filter = 'all' | 'pending' | 'completed';

@Component({
  selector: 'app-task-list',
  imports: [CommonModule, FormsModule],
  templateUrl: './task-list.html',
  styleUrl: './task-list.css',
})
export class TaskList {
  private router = inject(Router);
  private taskService = inject(TaskService);
  toast = inject(ToastService);
  readonly filterOptions: Filter[] = ['all', 'pending', 'completed'];

  tasks   = signal<TaskItem[]>([]);
  loading = signal(true);
  filter  = signal<Filter>('all');

  // computed() automatically recalculates whenever tasks() or filter() changes.
  // You don't call it manually — Angular tracks the dependency for you.
  filteredTasks = computed(() => {
    const f = this.filter();
    const all = this.tasks();
    if (f === 'completed') return all.filter(t => t.isCompleted);
    if (f === 'pending')   return all.filter(t => !t.isCompleted);
    return all;
  });

  total     = computed(() => this.tasks().length);
  pending   = computed(() => this.tasks().filter(t => !t.isCompleted).length);
  completed = computed(() => this.tasks().filter(t => t.isCompleted).length);

  skeletons = [1, 2, 3, 4]; // just used to repeat a skeleton row 4 times

  constructor() { this.loadTasks(); }

  loadTasks() {
    this.loading.set(true);
    this.taskService.getTasks().subscribe({
      next: res => { this.tasks.set(res); this.loading.set(false); },
      error: ()  => { this.loading.set(false); this.toast.error('Could not load tasks.'); }
    });
  }

  toggleComplete(task: TaskItem) {
    // We flip isCompleted locally first (optimistic update — feels instant)
    const updated = { ...task, isCompleted: !task.isCompleted };
    this.tasks.update(all => all.map(t => t.id === task.id ? updated : t));

    this.taskService.updateTask(task.id, updated).subscribe({
      next: () => {
        const msg = updated.isCompleted ? 'Task completed!' : 'Task marked as pending.';
        this.toast.success(msg);
      },
      error: () => {
        // If the API call failed, revert the local change
        this.tasks.update(all => all.map(t => t.id === task.id ? task : t));
        this.toast.error('Failed to update status.');
      }
    });
  }

  deleteTask(task: TaskItem) {
    this.taskService.deleteTask(task.id).subscribe({
      next: () => {
        this.tasks.update(all => all.filter(t => t.id !== task.id));
        this.toast.info(`"${task.title}" deleted.`);
      },
      error: () => this.toast.error('Failed to delete task.')
    });
  }

  goAdd(){ this.router.navigate(['/tasks/add']); }
  goEdit(id: string){ this.router.navigate(['/tasks/edit', id]); }
  setFilter(f: Filter) { this.filter.set(f); }
}
import { Component, signal } from '@angular/core';
import { TaskService } from '../../services/task.service';
import { TaskItem } from '../../models/task.model';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-task-list',
  imports: [CommonModule, FormsModule],
  templateUrl: './task-list.html',
  styleUrl: './task-list.css',
})
export class TaskList {
  tasks = signal<TaskItem[]>([]);

  constructor(private router: Router,private taskService: TaskService) {
    this.loadTask();
  }

  loadTask(){
    this.taskService.getTasks().subscribe(res => {
      this.tasks.set(res);
    });
  }

  deleteTask(id: number){
    this.taskService.deleteTask(id).subscribe(() => {
      this.loadTask();
    });
  }

  toggleComplete(task: any){
    this.taskService.updateTask(task.id, task).subscribe();
  }

  updateTask(task: any){
    this.taskService.updateTask(task.id, task).subscribe();
  }

  editTask(id: number){
    this.router.navigate(['/edit', id])
  }

  gotoAdd(){
    this.router.navigate(['/add']);
  }
}

import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TaskItem } from '../../models/task.model';
import { Route, Router } from '@angular/router';
import { TaskService } from '../../services/task.service';

@Component({
  selector: 'app-add-task',
  imports: [FormsModule, CommonModule],
  templateUrl: './add-task.html',
  styleUrl: './add-task.css',
})
export class AddTask {
  task: TaskItem = {
    id: 0,
    title: '',
    description: '',
    isCompleted: false
  }

  constructor(private router: Router, private taskService: TaskService) {}

  addTask(){
    this.taskService.addTask(this.task).subscribe(() => {
      this.router.navigate(['/']);
    });
  }

  goBack(){
    this.router.navigate(['/']);
  }
}

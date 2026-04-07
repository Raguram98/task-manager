import { ChangeDetectorRef, Component, OnInit, signal, Signal } from '@angular/core';
import { TaskItem } from '../../models/task.model';
import { ActivatedRoute, Router } from '@angular/router';
import { TaskService } from '../../services/task.service';
import { FormsModule } from "@angular/forms";

@Component({
  selector: 'app-edit-task',
  imports: [FormsModule],
  templateUrl: './edit-task.html',
  styleUrl: './edit-task.css',
})
export class EditTask implements OnInit {
  task= signal<TaskItem | null>(null);

  constructor(private route: ActivatedRoute, private taskService: TaskService, private router: Router, private cd: ChangeDetectorRef ) {}
  
  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    this.taskService.getTask(id).subscribe(res => {
      console.log("DATA:", res);
      this.task.set(res); 
    });
  }

  updateTask(){
    const t = this.task()
    if (!t) {
      alert("Task not loaded yet");
      return;
    }

    this.taskService.updateTask(t.id, t).subscribe(() => {
      this.router.navigate(['/']);
    });
  }
  
  goBack(){
    this.router.navigate(['/']);
  }
}

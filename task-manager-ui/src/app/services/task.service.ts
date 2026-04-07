import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TaskItem } from '../models/task.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class TaskService {
  private baseUrl = `${environment.apiUrl}/tasks`;

  constructor(private http: HttpClient){}

  getTasks(){
    return this.http.get<TaskItem[]>(this.baseUrl);
  }

  getTask(id: number){
    return this.http.get<TaskItem>(`${this.baseUrl}/${id}`);
  }

  addTask(task: TaskItem){
    return this.http.post(this.baseUrl, task);
  }

  updateTask(id: number, task: TaskItem){
    return this.http.put(`${this.baseUrl}/${id}`, task);
  }

  deleteTask(id: number){
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
}

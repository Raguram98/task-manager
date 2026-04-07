import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TaskItem } from '../models/task.model';

@Injectable({
  providedIn: 'root',
})
export class TaskService {
  private baseUrl = 'https://localhost:7090/api/tasks';

  constructor(private http: HttpClient){}

  getTasks(){
    return this.http.get<TaskItem[]>(this.baseUrl);
  }

  getTask(id: number){
    return this.http.get<TaskItem>(`${this.baseUrl}/${id}`);
  }

  addTask(task: any){
    return this.http.post(this.baseUrl, task);
  }

  updateTask(id: number, task: any){
    return this.http.put(`${this.baseUrl}/${id}`, task);
  }

  deleteTask(id: number){
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
}

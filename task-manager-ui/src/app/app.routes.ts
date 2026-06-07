import { Routes } from '@angular/router';
import { TaskList } from './components/task-list/task-list';
import { AddTask } from './components/add-task/add-task';
import { EditTask } from './components/edit-task/edit-task';
import { Auth } from './components/auth/auth';
import { authGuard } from './guards/auth-guard';
import { Landing } from './components/landing/landing';
import { redirectGuard } from './guards/redirect-guard';

export const routes: Routes = [
  {path: '', component: Landing, canActivate: [redirectGuard]},
  {path: 'login', component: Auth, data: {mode: 'login'}},
  {path: 'register', component: Auth, data: {mode: 'register'}},
  {path: 'tasks', component: TaskList, canActivate: [authGuard]},
  {path: 'tasks/add', component: AddTask, canActivate: [authGuard]},
  {path: 'tasks/edit/:id', component: EditTask, canActivate: [authGuard]},
  {path: '**', redirectTo: ''}
];
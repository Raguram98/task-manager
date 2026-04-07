import { Routes } from '@angular/router';
import { TaskList } from './components/task-list/task-list';
import { AddTask } from './components/add-task/add-task';
import { EditTask } from './components/edit-task/edit-task';

export const routes: Routes = [
    {path: '', component: TaskList},
    {path: 'add', component: AddTask},
    {path: 'edit/:id', component: EditTask}
];

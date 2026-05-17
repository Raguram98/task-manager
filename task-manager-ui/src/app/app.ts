import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Header } from "./shared/header/header";
import { Loading } from './shared/loading/loading';
import { Toast } from './shared/toast/toast';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Header, Loading, Toast],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {}

import { Component } from '@angular/core';
import { MovementComponent } from '../movement/movement.component';
import { MatGridListModule } from '@angular/material/grid-list'

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [MovementComponent, MatGridListModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {

}

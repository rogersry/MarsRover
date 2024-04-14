import { Component } from '@angular/core';
import { MatButton } from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import { RoverService } from '../rover.service';
import { MoveRoverCommand } from '../model/MoveRoverCommand';
import { MoveRoverCommandStep } from '../model/MoveRoverCommandStep';

@Component({
  selector: 'app-movement',
  standalone: true,
  imports: [MatIconModule],
  templateUrl: './movement.component.html',
  styleUrl: './movement.component.css'
})
export class MovementComponent {
  
  constructor(private roverService: RoverService) {
    
  }

  moveRover(direction: number, milliseconds: number) : void {
    let moveForwardStep : MoveRoverCommandStep = { 
      direction: direction,
      milliseconds:milliseconds
    };
    let moveRoverCommand : MoveRoverCommand = {
      moveRoverCommandSteps: [moveForwardStep]
    };
    console.log("Calling roverService to move rover");
    this.roverService.moveRover(moveRoverCommand).subscribe();
  }
}

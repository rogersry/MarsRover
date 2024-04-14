import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, map, retry, throwError } from 'rxjs';
import { MoveRoverCommand } from './model/MoveRoverCommand';

@Injectable({
  providedIn: 'root'
})
export class RoverService {

  constructor(private http: HttpClient) { }

  baseUrl = 'https://localhost:32768/api/Rover/'

  moveRover(moveRoverCommand: MoveRoverCommand): Observable<boolean> {
    console.log("calling api with " + JSON.stringify(moveRoverCommand));
    return this.http.post<any>(this.baseUrl + 'move', moveRoverCommand)
    .pipe(
      map( response => {
        console.log("Response from api: " + response);
        return response;
      }),
      retry(3), // retry a failed request up to 3 times
      catchError(this.handleError) // then handle the error
    );
  }

  private handleError(error: HttpErrorResponse) {
    if (error.status === 0) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong.
      console.error(
        `Backend returned code ${error.status}, body was: `, error.error);
    }
    // Return an observable with a user-facing error message.
    return throwError(() => new Error('Unknown error; please try again later.'));
  }
}

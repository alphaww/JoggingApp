import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Router, NavigationExtras } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs/operators';
import { ValidationErrorsService } from '../services/validation.errors.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toastr: ToastrService, private validationService: ValidationErrorsService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(error => {
        if (error) {
          switch (error.status) {
            case 400:
              if (error.error instanceof Object) { 
                const modalStateErrors = [];
                for (const key in error.error) {
                  if (error.error[key]) {
                    let perPropErrorArray = error.error[key]
                    for (const index in perPropErrorArray) {
                      modalStateErrors.push(perPropErrorArray[index])
                    }
                  }
                }
                this.validationService.show(modalStateErrors)
              } else if (error.error instanceof String) {
                this.toastr.error(error.error, error.status)
              }
              break;
            case 401:
              this.toastr.error(error.statusText, error.status);
              break;
            case 404:
              this.toastr.error(error.error, error.status);
              break;
            case 409:
              this.toastr.error(error.error, error.status);
              break;
            case 500:
              this.toastr.error('Something unexpected went wrong');
              console.log(error);
              break;
            default:
              this.toastr.error('Something unexpected went wrong');
              console.log(error);
              break;
          }
        }
        return throwError(error);
      })
    )
  }
}

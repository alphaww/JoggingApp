import { Injectable } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Observable } from 'rxjs';
import { ConfirmDialogComponent } from '../modals/confirm-dialog/confirm-dialog.component';
import { ValidationErrorsDialogComponent } from '../modals/validation-errors-dialog/validation-errors-dialog.component';

@Injectable({
  providedIn: 'root'
})
export class ValidationErrorsService {
  bsModelRef: BsModalRef;

  constructor(private modalService: BsModalService) { }

  show(validationErrorrs: any): Observable<boolean> {
      const config = {
        initialState: {
          message: validationErrorrs
        },
        class: 'modal-lg'
      }
    this.bsModelRef = this.modalService.show(ValidationErrorsDialogComponent, config);
    
    return new Observable<boolean>(this.getResult());
  }

  private getResult() {
    return (observer) => {
      const subscription = this.bsModelRef.onHidden.subscribe(() => {
        observer.next(this.bsModelRef.content.result);
        observer.complete();
      });

      return {
        unsubscribe() {
          subscription.unsubscribe();
        }
      }
    }
  }
}

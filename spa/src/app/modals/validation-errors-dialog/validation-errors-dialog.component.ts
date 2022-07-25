import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'validation-errors-dialog',
  templateUrl: './validation-errors-dialog.component.html',
  styleUrls: ['./validation-errors-dialog.component.css']
})
export class ValidationErrorsDialogComponent implements OnInit {
  title: string;
  message: any;

  constructor(public bsModalRef: BsModalRef) { }

  ngOnInit(): void {
  }

  confirm() {
    this.bsModalRef.hide();
  }

}

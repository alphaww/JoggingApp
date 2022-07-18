import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { JogService } from 'src/app/services/jog.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-jogs-insert',
  templateUrl: './jogs-insert.component.html',
  styleUrls: ['./jogs-insert.component.css']
})
export class JogsInsertComponent implements OnInit {
  @Output() cancelSaveMode = new EventEmitter();
  jogInsertForm: FormGroup;
  validationErrors: string[] = [];

  constructor(private jogService: JogService, private toastr: ToastrService, private fb: FormBuilder, private router: Router) {
    this.jogInsertForm = this.fb.group({
      date: ['', Validators.required],
      distance: ['', Validators.required],
      time: ['', Validators.required],
      latitude: ['', Validators.required],
      longitude: ['', Validators.required]
    })
  }

  ngOnInit(): void {
  }

  insert() {
    this.jogService.insert(this.jogInsertForm.value).subscribe(response => {
      this.toastr.show('successfully inserted a new jog entry.')
    }, error => {
      this.validationErrors = error;
    })
  }

  cancel() {
    this.cancelSaveMode.emit(false);
  }
}

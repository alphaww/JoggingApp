import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ConfirmService } from 'src/app/services/confirm.service';
import { JogService } from 'src/app/services/jog.service';

@Component({
  selector: 'app-jogs-insert',
  templateUrl: './jogs-insert.component.html',
  styleUrls: ['./jogs-insert.component.css']
})
export class JogsInsertComponent implements OnInit {
  jogInsertForm: FormGroup;
  coordinates: FormGroup;
  time: FormGroup;
  currentDate = new Date();

  constructor(private jogService: JogService, private toastr: ToastrService, private fb: FormBuilder, private router: Router) {
    this.jogInsertForm = this.fb.group({
      distance: ['', Validators.required],
      // time: ['', Validators.required],
      coordinates: this.fb.group({
        latitude: [''],
        longitude: ['']
      }),
      time: this.fb.group({
        hours: ['',  Validators.required],
        minutes: ['',  Validators.required],
        seconds: ['',  Validators.required]
      })
    })
    this.coordinates = this.jogInsertForm.controls['coordinates'] as FormGroup
    this.time = this.jogInsertForm.controls['time'] as FormGroup
  }

  ngOnInit(): void {
  }

  insert() {
    //This is probably a hack. There is surely a more elegant solution to handle nested groups in FormGroup to propagate null.
    if (!this.jogInsertForm.value.coordinates.latitude || !this.jogInsertForm.value.coordinates.longitude) {
        this.toastr.warning('You have not entered either latitude or longitude, or both. Weather info will be unavailable for this jog entry.', 'Weather info unavailable');
        this.jogInsertForm.value.coordinates = null;
    }
    this.jogService.insert(this.jogInsertForm.value)
    .subscribe({
      next: (response) => {
        this.toastr.success('Successfully inserted a new jog record.', 'Jog entry inserted')
        this.router.navigateByUrl('jogs')
      },
      error: (e) => console.error(e)
    });
  }

  cancel() {
    this.router.navigateByUrl('jogs')
  }
}

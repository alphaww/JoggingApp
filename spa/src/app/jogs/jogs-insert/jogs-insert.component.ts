import { Component,  OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
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
      distance: ['', [Validators.required, Validators.min(0), Validators.max(500000)]],
      coordinates: this.fb.group({
        latitude: ['', [Validators.min(-90), Validators.max(90)]],
        longitude: ['', [Validators.min(-180), Validators.max(180)]]
      }),
      time: this.fb.group({
        hours: ['', [Validators.required, Validators.min(0), Validators.max(23)]],
        minutes: ['', [Validators.required, Validators.min(0), Validators.max(59)]],
        seconds: ['', [Validators.required, Validators.min(0), Validators.max(59)]]
      })
    })
    this.coordinates = this.jogInsertForm.controls['coordinates'] as FormGroup
    this.time = this.jogInsertForm.controls['time'] as FormGroup
  }

  ngOnInit(): void {
  }

  insert() {

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

  fillInCoordinates() {
    if(navigator.geolocation){
           navigator.geolocation.getCurrentPosition(position => {
            const coordinates = {
              latitude: position.coords.latitude,
              longitude: position.coords.longitude
            }
            this.coordinates.patchValue(coordinates);
           }, 
           error => {
            this.toastr.warning('Your browser does not support geo location.', 'Geo location unsupported');
           });
        }

  }
}

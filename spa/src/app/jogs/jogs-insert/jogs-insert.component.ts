import { Component, EventEmitter, OnInit, Output } from '@angular/core';
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
  currentDate = new Date();

  constructor(private jogService: JogService, private toastr: ToastrService, private fb: FormBuilder, private router: Router) {
    this.jogInsertForm = this.fb.group({
      distance: ['', Validators.required],
      time: ['', Validators.required],
      coordinates: this.fb.group({
        latitude: [''],
        longitude: ['']
      })
    })
    this.coordinates = this.jogInsertForm.controls['coordinates'] as FormGroup
  }

  ngOnInit(): void {
  }

  insert() {
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

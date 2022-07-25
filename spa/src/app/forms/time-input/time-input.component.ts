import { Component, Input, OnInit, Self } from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';


@Component({
  selector: 'app-time-input',
  templateUrl: './time-input.component.html',
  styleUrls: ['./time-input.component.css']
})
export class TimeInputComponent implements ControlValueAccessor  {
  @Input() label: string;
  private onChange: Function;

  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this;
  }

  ngOnInit() {
     this.ngControl.valueChanges
      .subscribe((v) => {
        if (v && typeof v.getMonth === 'function') {
         this.onChange(this.transform(v));
        }
      })
  }

  writeValue(obj: any): void {
    obj = new Date();
  }
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
  }
  setDisabledState?(isDisabled: boolean): void {

  }

  private transform(val: Date) {
    let time = val.getHours() + ':' + val.getMinutes() + ':' + val.getSeconds();
    return time;
  }

}

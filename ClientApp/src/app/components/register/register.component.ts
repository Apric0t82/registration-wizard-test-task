import { Component, OnInit, inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { LocationService } from '../../services/api/location.service';
import { AuthService } from '../../services/api/auth.service';
import { ValidationService } from '../../services/validation.service';
import { CommonModule } from '@angular/common';
import { MatCard } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { SnackbarService } from '../../services/snackbar.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCard,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatCheckboxModule,
  ],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  private router = inject(Router);
  private snack = inject(SnackbarService);

  step = 1;
  registerFormStep1: FormGroup;
  registerFormStep2: FormGroup;
  countries: any[] = [];
  provinces: any[] = [];
  validationErrors?: string[];

  constructor(
    private locationService: LocationService,
    private authService: AuthService,
    private validationService: ValidationService
  ) {
    this.registerFormStep1 = new FormGroup(
      {
        email: new FormControl('', [Validators.required, Validators.email]),
        password: new FormControl('', [
          Validators.required,
          Validators.pattern(/^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$/),
        ]),
        confirmPassword: new FormControl(
          '',
          Validators.compose([Validators.required])
        ),
        agree: new FormControl(false, Validators.requiredTrue),
      },
      this.validationService.passwordMatch('password', 'confirmPassword')
    );

    this.registerFormStep2 = new FormGroup({
      country: new FormControl('', Validators.required),
      province: new FormControl('', Validators.required),
    });
  }

  ngOnInit(): void {
    this.loadCountries();
  }

  loadCountries() {
    this.locationService.getCountries().subscribe((countries) => {
      this.countries = countries;
    });
  }

  onCountryChange(countryId: number) {
    this.locationService.getProvinces(countryId).subscribe((provinces) => {
      this.provinces = provinces;
      this.registerFormStep2.controls['province'].setValue('');
    });
  }

  nextStep() {
    if (this.registerFormStep1.invalid) {
      this.registerFormStep1.markAllAsTouched();
      return;
    }
    this.step = 2;
  }

  submit() {
    if (this.registerFormStep2.invalid) {
      this.registerFormStep2.markAllAsTouched();
      return;
    }

    const userData = {
      email: this.registerFormStep1.value.email,
      password: this.registerFormStep1.value.password,
      countryId: this.registerFormStep2.value.country,
      provinceId: this.registerFormStep2.value.province,
    };

    this.authService.register(userData).subscribe({
      next: () => {
        this.snack.success('Registration successful! Now you can log in.');
        this.router.navigateByUrl('/login');
      },
      error: errors => this.validationErrors = errors
    });
  }
}

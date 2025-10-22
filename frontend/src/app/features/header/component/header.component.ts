import { Component } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { Router, RouterModule } from '@angular/router';
import { AsyncPipe, NgIf } from '@angular/common';
import { Observable } from 'rxjs';
import { UserService } from '../../../core/services/user.service';
import { UserDto } from '../../../core/models/user.model';
import { CreateButtonComponent } from '../../../shared/components/button/create-button/create-button.component';
  
@Component({
  selector: 'app-header',
  imports: [ NgIf, AsyncPipe, RouterModule,  CreateButtonComponent],
  templateUrl: './header.component.html',
})

export class HeaderComponent {
  currentUser$!: Observable<UserDto | null>;

  constructor(private userService: UserService, private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.currentUser$ = this.userService.getCurrentUser();
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }
}
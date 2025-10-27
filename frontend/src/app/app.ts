import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TokenService } from './core/services/token.service';
import { AuthService } from './core/services/auth.service';
import { UserService } from './core/services/user.service';
import { HeaderComponent } from './layout/header/component/header.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, HeaderComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('frontend');

  constructor(private tokenService: TokenService, private authService: AuthService, private userService: UserService) { }

  ngOnInit() {
    const token = this.tokenService.getAccessToken();
    if (token) {
      this.userService.loadCurrentUser().subscribe({
        error: () => {
          this.authService.logout(true);
        }
      });
    }
  }
}
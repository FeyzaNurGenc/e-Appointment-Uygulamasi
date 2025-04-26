import { Component } from '@angular/core';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-navbar',

  //imports: [RouterLink],
  standalone: true, // Eğer standalone bileşen kullanıyorsan eklemelisin.
  imports: [CommonModule, RouterModule], // CommonModule ve RouterModule EKLENDİ
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent {
  constructor(private router: Router, public auth: AuthService) {}
  signOut() {
    localStorage.removeItem('token');
    this.router.navigateByUrl('/login');
  }
}

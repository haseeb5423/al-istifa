import { inject } from '@angular/core';
import { CanActivateFn, CanActivateChildFn, Router } from '@angular/router';
import { AuthService } from '@features/authentication/services/auth.service';
import { ToastrService } from 'ngx-toastr';

const checkAuth = () => {
  const authSvc = inject(AuthService);
  const toastr = inject(ToastrService);
  const router = inject(Router);

  if (!authSvc.AuthCheck.value) {
    toastr.warning('You must be logged in to access this page.');
    router.navigate(['/login']);
    return false;
  }
  return true;
};

export const canActivateGuard: CanActivateFn = (route, state) => checkAuth();
export const canActivateChildGuard: CanActivateChildFn = (route, state) => checkAuth();

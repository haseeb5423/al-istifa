import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '@features/authentication/services/auth.service';
import { ToastrService } from 'ngx-toastr';

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const excludedUrls = ['/auth/login', '/auth/register', '/auth/register/step1', '/auth/register/step2'];
  const authSvc = inject(AuthService);
  const router = inject(Router);
  const toastr = inject(ToastrService);
  const authToken = authSvc.getToken();

  const shouldSkip = excludedUrls.some(url => req.url.includes(url));
  if (shouldSkip) {
    console.log('Skipping token attachment for URL:', req.url);
    return next(req); 
  }
   if (!authSvc.getToken() || authSvc.isTokenExpired()) {
    toastr.warning('Session expired. Please log in again.');
    router.navigate(['/login']);
  }
   if (authToken) {
    const clonedReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${authToken}`
      }
    });
    return next(clonedReq);
  }

  return next(req);
};

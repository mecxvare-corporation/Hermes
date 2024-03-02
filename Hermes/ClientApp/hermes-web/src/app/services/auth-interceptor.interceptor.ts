import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptorInterceptor: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem('acctoken');
  // This will add headers to all requests, this is not a good idea,
  // we can add maybe if statment to check if URL starts with our api.
  if (token) {
    const headers = req.headers.set('Authorization', `Bearer ${token}`);
    const authReq = req.clone({ headers });
    return next(authReq);
  }
  return next(req);
};

export interface LoginDto {
  email: string,
  password: string
}

export interface RegisterDto {
  fullname: string;
  email: string;
  password: string;
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
}
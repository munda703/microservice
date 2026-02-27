export interface RegisterDTO {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}

export interface LoginDTO {
  email: string;
  password: string;
}

export interface AuthResponseDTO {
  accessToken: string;
}

export interface UserDTO {
  firstName: string;
  lastName: string;
  email: string;
}

export interface ApiError {
  title?: string;
  status?: number;
  errors?: Record<string, string[]>;
  detail?: string;
}
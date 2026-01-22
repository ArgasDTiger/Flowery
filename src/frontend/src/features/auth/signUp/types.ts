export interface SignUpRequest {
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber?: string | null;
  password: string;
}

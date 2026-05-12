export type ApiStatus = "success" | "failed";

export interface ApiResponse<T = null> {
  status: ApiStatus;
  message: string;
  data: T;
}

export interface ApiMessageResponse {
  status: string;
  message: string;
}

export interface ApiErrorResponse {
  status?: string;
  message?: string;
  errors?: Record<string, string[]>;
}

export interface AppError {
  statusCode?: number;
  message: string;
  errors?: Record<string, string[]>;
}

export type RequestStatus = "idle" | "loading" | "succeeded" | "failed";

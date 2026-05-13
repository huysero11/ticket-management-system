import type { AppError } from "../types/common";

function isStringArray(value: unknown): value is string[] {
  return (
    Array.isArray(value) && value.every((item) => typeof item === "string")
  );
}

function isValidationErrors(value: unknown): value is Record<string, string[]> {
  if (typeof value !== "object" || value === null) {
    return false;
  }

  return Object.values(value).every(isStringArray);
}

export function isAppError(error: unknown): error is AppError {
  if (error instanceof Error) {
    return false;
  }

  if (typeof error !== "object" || error === null) {
    return false;
  }

  const possibleError = error as {
    statusCode?: unknown;
    message?: unknown;
    errors?: unknown;
  };

  const hasValidMessage = typeof possibleError.message === "string";

  const hasValidStatusCode =
    possibleError.statusCode === undefined ||
    typeof possibleError.statusCode === "number";

  const hasValidErrors =
    possibleError.errors === undefined ||
    isValidationErrors(possibleError.errors);

  return hasValidMessage && hasValidStatusCode && hasValidErrors;
}

export function toAppError(
  error: unknown,
  fallbackMessage = "Something went wrong.",
): AppError {
  if (isAppError(error)) {
    return error;
  }

  if (error instanceof Error) {
    return {
      message: error.message,
    };
  }

  return {
    message: fallbackMessage,
  };
}

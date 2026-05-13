import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { toAppError } from "../../services/errorHandler";
import { tokenStorage } from "../../services/tokenStorage";
import type { AppError } from "../../types/common";
import { authApi } from "./authApi";
import type {
  AuthResponse,
  AuthState,
  LoginRequest,
  RegisterRequest,
  RegisterResponse,
} from "./authTypes";

const initialUser = tokenStorage.getUser();
const initialAccessToken = tokenStorage.getAccessToken();
const initialRefreshToken = tokenStorage.getRefreshToken();

const initialState: AuthState = {
  user: initialUser,
  accessToken: initialAccessToken,
  refreshToken: initialRefreshToken,
  isAuthenticated: Boolean(initialUser && initialAccessToken),
  isLoading: false,
  error: null,
};

export const loginThunk = createAsyncThunk<
  AuthResponse,
  LoginRequest,
  { rejectValue: AppError }
>("auth/login", async (payload, { rejectWithValue }) => {
  try {
    const response = await authApi.login(payload);
    return response.data;
  } catch (error: unknown) {
    return rejectWithValue(toAppError(error, "Failed to login."));
  }
});

export const registerThunk = createAsyncThunk<
  RegisterResponse,
  RegisterRequest,
  { rejectValue: AppError }
>("auth/register", async (payload, { rejectWithValue }) => {
  try {
    const response = await authApi.register(payload);
    return response.data;
  } catch (error: unknown) {
    return rejectWithValue(toAppError(error, "Failed to register."));
  }
});

export const logoutThunk = createAsyncThunk<
  void,
  void,
  { rejectValue: AppError }
>("auth/logout", async (_, { rejectWithValue }) => {
  const refreshToken = tokenStorage.getRefreshToken();

  try {
    if (refreshToken) {
      await authApi.logout(refreshToken);
    }

    tokenStorage.clearAuthData();
  } catch (error: unknown) {
    tokenStorage.clearAuthData();
    return rejectWithValue(toAppError(error, "Failed to logout."));
  }
});

const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    clearAuthError(state) {
      state.error = null;
    },

    clearAuthState(state) {
      state.user = null;
      state.accessToken = null;
      state.refreshToken = null;
      state.isAuthenticated = false;
      state.isLoading = false;
      state.error = null;

      tokenStorage.clearAuthData();
    },
  },
  extraReducers: (builder) => {
    builder
      // Login
      .addCase(loginThunk.pending, (state) => {
        state.isLoading = true;
        state.error = null;
      })
      .addCase(loginThunk.fulfilled, (state, action) => {
        const { accessToken, refreshToken, user } = action.payload;

        state.user = user;
        state.accessToken = accessToken;
        state.refreshToken = refreshToken;
        state.isAuthenticated = true;
        state.isLoading = false;
        state.error = null;

        tokenStorage.saveAuthData(accessToken, refreshToken, user);
      })
      .addCase(loginThunk.rejected, (state, action) => {
        state.isLoading = false;
        state.error = action.payload ?? {
          message: "Failed to login.",
        };
      })

      // Register
      .addCase(registerThunk.pending, (state) => {
        state.isLoading = true;
        state.error = null;
      })
      .addCase(registerThunk.fulfilled, (state) => {
        state.isLoading = false;
        state.error = null;

        // Option 1:
        // Register only creates account.
        // It does not login user because backend does not return tokens.
      })
      .addCase(registerThunk.rejected, (state, action) => {
        state.isLoading = false;
        state.error = action.payload ?? {
          message: "Failed to register.",
        };
      })

      // Logout
      .addCase(logoutThunk.pending, (state) => {
        state.isLoading = true;
      })
      .addCase(logoutThunk.fulfilled, (state) => {
        state.user = null;
        state.accessToken = null;
        state.refreshToken = null;
        state.isAuthenticated = false;
        state.isLoading = false;
        state.error = null;
      })
      .addCase(logoutThunk.rejected, (state, action) => {
        state.user = null;
        state.accessToken = null;
        state.refreshToken = null;
        state.isAuthenticated = false;
        state.isLoading = false;
        state.error = action.payload ?? {
          message: "Failed to logout.",
        };
      });
  },
});

export const { clearAuthError, clearAuthState } = authSlice.actions;

export default authSlice.reducer;

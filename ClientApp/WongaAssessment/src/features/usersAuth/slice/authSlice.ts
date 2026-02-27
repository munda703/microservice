import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import axios from "axios";
import { loginUser, registerUser, getMyDetails } from "../api/authApi";
import type { UserDTO, AuthResponseDTO, RegisterDTO, LoginDTO, ApiError } from "../types";

interface AuthState {
  user: UserDTO | null;
  isAuthenticated: boolean;

  registerLoading: boolean;
  loginLoading: boolean;
  userLoading: boolean;

  error: ApiError | null;
}

const initialState: AuthState = {
  user: null,
  isAuthenticated: !!localStorage.getItem("token"),

  registerLoading: false,
  loginLoading: false,
  userLoading: false,

  error: null,
};

export const register = createAsyncThunk<
  AuthResponseDTO,
  RegisterDTO,
  { rejectValue: ApiError }
>("gateWay/register", async (data, { rejectWithValue }) => {
  try {
    return await registerUser(data);
  } catch (error) {
    if (axios.isAxiosError(error) && error.response?.data) {
      return rejectWithValue(error.response.data);
    }
    return rejectWithValue({
      title: "Network error",
      detail: "Unable to reach server",
    });
  }
});

export const login = createAsyncThunk<
  AuthResponseDTO,
  LoginDTO,
  { rejectValue: ApiError }
>("gateWay/login", async (data, { rejectWithValue }) => {
  try {
    const response = await loginUser(data);
    localStorage.setItem("token", response.accessToken);
    return response;
  } catch (error) {
    if (axios.isAxiosError(error) && error.response?.data) {
      return rejectWithValue(error.response.data);
    }
    return rejectWithValue({
      title: "Network error",
      detail: "Unable to reach server",
    });
  }
});

export const fetchUser = createAsyncThunk<
  UserDTO,
  void,
  { rejectValue: ApiError }
>("auth/fetchUser", async (_, { rejectWithValue }) => {
  try {
    return await getMyDetails();
  } catch (error) {
    if (axios.isAxiosError(error) && error.response?.data) {
      return rejectWithValue(error.response.data);
    }
    return rejectWithValue({
      title: "Error",
      detail: "Unable to fetch user",
    });
  }
});


const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    clearError: (state) => {
      state.error = null;
    },
    logout: (state) => {
      localStorage.removeItem("token");
      state.isAuthenticated = false;
      state.user = null;
    },
  },
  extraReducers: (builder) => {

    builder
      .addCase(register.pending, (state) => {
        state.registerLoading = true;
        state.error = null;
      })
      .addCase(register.fulfilled, (state) => {
        state.registerLoading = false;
      })
      .addCase(register.rejected, (state, action) => {
        state.registerLoading = false;
        state.error = action.payload ?? null;
      });

    builder
      .addCase(login.pending, (state) => {
        state.loginLoading = true;
        state.error = null;
      })
      .addCase(login.fulfilled, (state) => {
        state.loginLoading = false;
        state.isAuthenticated = true;
      })
      .addCase(login.rejected, (state, action) => {
        state.loginLoading = false;
        state.error = action.payload ?? null;
      });

    builder
      .addCase(fetchUser.pending, (state) => {
        state.userLoading = true;
      })
      .addCase(fetchUser.fulfilled, (state, action) => {
        state.userLoading = false;
        state.user = action.payload;
      })
      .addCase(fetchUser.rejected, (state) => {
        state.userLoading = false;
        state.user = null;
      });
  },
});

export const { clearError, logout } = authSlice.actions;
export default authSlice.reducer;
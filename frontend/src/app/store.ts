import { configureStore } from "@reduxjs/toolkit";
import authReducer from "../features/auth/authSlice";
import ticketCategoryReducer from "../features/ticketCategories/ticketCategorySlice";

export const store = configureStore({
  reducer: {
    auth: authReducer,
    ticketCategories: ticketCategoryReducer,
    // tickets: ticketsReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

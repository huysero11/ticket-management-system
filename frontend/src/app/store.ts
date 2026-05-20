import { configureStore } from "@reduxjs/toolkit";
import authReducer from "../features/auth/authSlice";
import ticketCategoryReducer from "../features/ticketCategories/ticketCategorySlice";
import ticketsReducer from "../features/tickets/ticketSlice";
import dashboardReducer from "../features/dashboard/dashboardSlice";

export const store = configureStore({
  reducer: {
    auth: authReducer,
    ticketCategories: ticketCategoryReducer,
    tickets: ticketsReducer,
    dashboard: dashboardReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

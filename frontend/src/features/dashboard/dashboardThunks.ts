import { createAsyncThunk } from "@reduxjs/toolkit";
import type { AppError } from "../../types/common";
import { toAppError } from "../../services/errorHandler";
import { dashboardApi } from "./dashboardApi";
import type { DashboardSummary } from "./dashboardTypes";

export const getDashboardSummaryThunk = createAsyncThunk<
    DashboardSummary,
    void,
    { rejectValue: AppError }
>("dashboard/getSummary", async (_, { rejectWithValue }) => {
    try {
        const response = await dashboardApi.getSummary();
        return response.data;
    } catch (error) {
        return rejectWithValue(toAppError(error));
    }
});
import axiosClient from "../../services/axiosClient";
import type { ApiResponse } from "../../types/common";
import type { DashboardSummary } from "./dashboardTypes";

const DASHBOARD_URL = "/dashboard";

export const dashboardApi = {
    getSummary(): Promise<ApiResponse<DashboardSummary>> {
        return axiosClient.get<
            ApiResponse<DashboardSummary>,
            ApiResponse<DashboardSummary>
        >(`${DASHBOARD_URL}/summary`);
    },
};
import { Navigate, Outlet } from "react-router-dom";
import { useAppSelector } from "../app/hooks";
import { selectCurrentUser } from "../features/auth/authSelectors";

interface RoleRouteProps {
  allowedRoles: string[];
}

export default function RoleRoute({ allowedRoles }: RoleRouteProps) {
  const currentUser = useAppSelector(selectCurrentUser);

  if (!currentUser) {
    return <Navigate to="/login" replace />;
  }

  if (!allowedRoles.includes(currentUser.role)) {
    return <Navigate to="/app/dashboard" replace />;
  }

  return <Outlet />;
}

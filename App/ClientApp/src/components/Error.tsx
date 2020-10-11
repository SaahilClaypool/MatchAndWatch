import React from "react";
import { useRouteMatch } from "react-router-dom";

export function NoRoute({ path }: { path: string }) {
  return (
    <div>
      Thats not right... could not find route <code>{path}</code>.
    </div>
  );
}

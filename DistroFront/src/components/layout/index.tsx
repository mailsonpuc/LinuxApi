import { Outlet } from "react-router-dom"
import { Header } from "../header"

export function Layuot() {
    return (
        <>
            <Header />
            <Outlet />
        </>
    )
}
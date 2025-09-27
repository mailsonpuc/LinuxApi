import { Link } from "react-router-dom"


export function NotFound() {
    return (
        <div>
            <h1>Opps, esssa pagina não existe.</h1>
            <Link to="/">Volta pra HOME</Link>
        </div>
    )
}
import { createBrowserRouter } from "react-router-dom";
import { Distros } from "./pages/distros";
import { AdicionarDistro } from "./pages/adicionarDistros";
import { NotFound } from "./pages/notfound";
import { Layuot } from "./components/layout";
import { AdicionarCategoria } from "./pages/adicionarCategoria";
import Login from "./pages/login";
import Registrar from "./pages/registrar";

// Importe o novo componente PrivateRoute
import { PrivateRoute } from "./components/PrivateRoute"; 


const router = createBrowserRouter([
    {
        // Rota Raiz que contém o Layout
        element: <Layuot />,
        children: [
            // 1. ROTAS PÚBLICAS
            {
                path: "/",
                element: <Distros /> // Exemplo: Página inicial
            },
            {
                path: "/login",
                element: <Login />
            },
            {
                path: "/registrar",
                element: <Registrar />
            },
            
            // 2. ROTAS PROTEGIDAS
            // Adicionamos o PrivateRoute como elemento pai
            {
                element: <PrivateRoute />, // A checagem de AUTH acontece AQUI
                children: [
                    {
                        path: "/adddistro",
                        element: <AdicionarDistro /> // Protegida
                    },
                    {
                        path: "/addcategoria",
                        element: <AdicionarCategoria /> // Protegida
                    },
                ]
            },

            // 3. Rota de Erro ( a última)
            {
                path: "*",
                element: <NotFound />
            }
        ]
    }
])


export { router };

import { Outlet, Navigate } from 'react-router-dom';

export function PrivateRoute() {
    // Verifica se o token de autenticação existe no localStorage
    // '!!' converte o valor (string ou null) para um booleano (true ou false)
    const isAuthenticated = !!localStorage.getItem('authToken');

    if (isAuthenticated) {
        // Se estiver logado, renderiza o conteúdo da rota aninhada
        return <Outlet />;
    }
    
    // Se NÃO estiver logado, redireciona para a página de login
    return <Navigate to="/login" replace />;
}
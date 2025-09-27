import "./header.css"
import { Link, useNavigate, useLocation } from 'react-router-dom'; // Importe useLocation AQUI!
import { useState, useEffect } from 'react';
import { jwtDecode } from "jwt-decode";


export function Header() {
    // Tipagem: 'loggedInUser' pode ser uma string (o nome) ou null
    const [loggedInUser, setLoggedInUser] = useState<string | null>(null);
    const navigate = useNavigate();
    const location = useLocation(); // Inicialize o hook useLocation AQUI!

    // Função para checar o login
    const checkAuthStatus = () => {
        const token = localStorage.getItem('authToken');

        if (token) {
            try {
                const decodedToken: any = jwtDecode(token);
                const userName = decodedToken.unique_name;

                setLoggedInUser(userName || null);
                localStorage.setItem('userName', userName);
            } catch (e) {
                console.error("Token inválido ou expirado. Limpando sessão.", e);
                localStorage.clear();
                setLoggedInUser(null);
            }
        } else {
            setLoggedInUser(null);
        }
    };

    useEffect(() => {
        // 1. Roda a checagem sempre que a rota (location) muda.
        // Isso resolve o problema de login na mesma aba.
        checkAuthStatus();

        // 2. Mantém o listener para mudanças em outras abas.
        window.addEventListener('storage', checkAuthStatus);

        return () => {
            window.removeEventListener('storage', checkAuthStatus);
        };

        // 💡 Ajuste Crucial: Adicione location.pathname como dependência.
    }, [location.pathname]);

    // Tipagem: o 'handleLogout' não precisa de argumentos
    const handleLogout = () => {
        localStorage.removeItem('userName');
        localStorage.removeItem('authToken');
        localStorage.removeItem('refreshToken');
        localStorage.removeItem('tokenExpiration');

        setLoggedInUser(null);

        // Ao invés de apenas navegar, você pode forçar a recarga do estado
        // do componente PrivateRoute (embora 'navigate' deva funcionar)
        navigate('/login', { replace: true });
    };


    return (
        <header>
            <h2>
                <Link to="/">Distros Linux App</Link>
            </h2>

            {loggedInUser ? (
                // Se o usuário estiver logado, exibe os links
                <nav className="user-info">
                    <span className='welcome-message'>Olá, {loggedInUser}!</span>
                    {/* <Link to="/">Home</Link> */}
                    {/* <Link to="/distros">Distros</Link> */}
                    <Link to="/adddistro">AddDistro</Link>
                    <Link to="/addcategoria">AddCategoria</Link>
                    <button className='logout-button' onClick={handleLogout}>Sair</button>
                </nav>
            ) : (
                // Se não estiver logado, exibe o link de login e registrar
                <nav className='auth-links'>
                    <Link className='login-link' to="/login">Login</Link>
                    <Link className='login-link' to="/registrar">Registrar</Link>
                </nav>
            )}
        </header>
    )
}

export default Header;
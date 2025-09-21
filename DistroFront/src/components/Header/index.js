import "./styles.css"
import { Link, useNavigate } from 'react-router-dom';
import { useState, useEffect } from 'react';



function Header() {
    const [loggedInUser, setLoggedInUser] = useState(null);
    const navigate = useNavigate();


    useEffect(() => {
        // Pega o nome de usuário do localStorage
        const storedUser = localStorage.getItem('userName');
        if (storedUser) {
            setLoggedInUser(storedUser);
        }
    }, [navigate]);



    const handleLogout = () => {
        // Remove os dados do usuário do localStorage
        localStorage.removeItem('userName');
        localStorage.removeItem('authToken');

        // Limpa o estado para esconder a mensagem de boas-vindas
        setLoggedInUser(null);

        // Redireciona o usuário para a página de login
        navigate('/login');
    };


    return (
        <header>
            <h2>Distros Linux App</h2>

            {loggedInUser ? (
                // Se o usuário estiver logado, exibe a mensagem de boas-vindas e o botão "Sair"
                <div className="user-info">
                    <span className='welcome-message'>Olá, {loggedInUser}!</span>
                    <Link to="/distros">Distros</Link>
                    <Link to="/adddistro">AddDistro</Link>
                    <Link to="/addcategoria">AddCategoria</Link>
                    <Link to="sobre">Sobre</Link>
                    <button className='logout-button' onClick={handleLogout}>Sair</button>
                </div>
            ) : (
                // Se não estiver logado, exibe o link de login e o link de registrar agrupados
                <div className='auth-links'>
                    <Link className='login-link' to="/login">Login</Link>
                    <Link className='login-link' to="/registrar">Registrar</Link>
                </div>
            )}


        </header>
    )
}

export default Header;


import './loading.css';


function Loading() {
    // REMOVIDO: const [loading, setLoading] = useState(true);
    // REMOVIDO: if (loading) { ... }

    // O componente sempre retorna a UI de carregamento quando é renderizado
    return (
        <div className="loading-container">
            <svg className="loading-progress">
                <circle r="40%" cx="50%" cy="50%" />
                <circle r="40%" cx="50%" cy="50%" />
            </svg>
            <div className="loading-progress-text">Carregando...</div>
        </div>
    )
}

export default Loading;
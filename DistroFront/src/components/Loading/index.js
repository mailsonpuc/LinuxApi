import { useState } from "react";
import './styles.css';


function Loading() {
    const [loading, setLoading] = useState(true);

    if (loading) {
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

}

export default Loading;
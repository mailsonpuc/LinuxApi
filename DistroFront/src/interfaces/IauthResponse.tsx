// Definição da interface para os dados de resposta EXATOS da API
export interface AuthResponse {
    token: string;
    refreshToken: string; 
    expiration: string;  
}

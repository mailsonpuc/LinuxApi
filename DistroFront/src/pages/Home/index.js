import { Link } from 'react-router-dom';


function Home() {

  return (
    <div>
      <h1>BEM VINDO A PAGINA HOME</h1>
      <Link to="/sobre">sobre</Link>
    </div>
  );
}

export default Home;

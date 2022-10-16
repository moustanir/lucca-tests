import React from 'react';
import './App.css';
// @ts-ignore
import { ChatBoxesContext } from './contexts/index.tsx';
import { ChatMessage } from './components/contracts';
import ChatBox from './components/ChatBox/ChatBox';

const App = () =>{
  const [state, setState] = React.useState<ChatMessage[]>([]);
  return (
    <ChatBoxesContext.Provider value={{messages:state, setMessages:setState}}>
      <div className="App">
        <ChatBox label="Chat A" author={"authorA"} />
        <ChatBox label="Chat B" author={"authorB"} />
      </div>
    </ChatBoxesContext.Provider>
  );
}

export default App;

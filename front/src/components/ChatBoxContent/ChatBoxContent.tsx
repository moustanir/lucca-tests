import React from 'react';
import { ChatMessage, ContentProps } from '../contracts';
import ChatBoxMessage from '../ChatBoxMessage/ChatBoxMessage';
import { ChatBoxesContext } from '../../contexts';

export const ChatBoxContent: React.FC<ContentProps> = ({ author }) => {
  const contextData = React.useContext(ChatBoxesContext);
  return (
    <div data-testid="chatBoxContent-container" className="ChatBoxMessageContainer">
      {(contextData !== undefined && contextData.messages !== undefined) && contextData.messages.map((message: ChatMessage, index: number) => {
        return (<ChatBoxMessage message={message} id={index} reader={author} />);
      })}
    </div>
  )
}

export default ChatBoxContent;
ChatBoxContent.displayName = 'ChatBoxContent';
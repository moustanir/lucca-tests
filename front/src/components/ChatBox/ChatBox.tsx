import React from 'react';
import { ChatBoxProps } from '../contracts';
import { ChatBoxContent } from '../ChatBoxContent';
import { ChatField } from '../ChatField';

export const ChatBox : React.FC<ChatBoxProps> = ({ label, author }) => {
    return (
      <div data-testid="chatbox-container" className="ChatBoxContainer">
        <label data-testid="chatbox-label">{label}</label>
        <ChatBoxContent author={author} />
        <ChatField author={author} />
      </div>
    );
  }

export default ChatBox;
ChatBox.displayName = 'ChatBox';
